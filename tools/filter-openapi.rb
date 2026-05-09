#!/usr/bin/env ruby
# frozen_string_literal: true

require "json"

WANTED_PATHS = ["/BankWizard", "/IDAML", "/IDAML/MONITOR"].freeze
BANKWIZARD_RESPONSE = "Arkitec.Veriphy.Common.Interfaces.CCBank.VeriphyCCBankCheckResponseTO"

def scan_refs(value, refs)
  case value
  when Hash
    ref = value["$ref"]
    refs << ref.delete_prefix("#/definitions/") if ref&.start_with?("#/definitions/")
    value.each_value { |child| scan_refs(child, refs) }
  when Array
    value.each { |child| scan_refs(child, refs) }
  end
end

source = JSON.parse($stdin.read)
definitions = source.fetch("definitions")
paths = source.fetch("paths").slice(*WANTED_PATHS)

# The live Swagger currently declares POST /BankWizard 200 as the request DTO.
# The GET operation and DTO naming indicate that the response DTO is intended.
paths.fetch("/BankWizard").fetch("post").fetch("responses").fetch("200")["schema"] = {
  "$ref" => "#/definitions/#{BANKWIZARD_RESPONSE}"
}

refs = []
paths.each_value { |path| scan_refs(path, refs) }

seen = {}
queue = refs.uniq

until queue.empty?
  name = queue.shift
  next if seen[name]

  schema = definitions.fetch(name)
  seen[name] = schema

  nested_refs = []
  scan_refs(schema, nested_refs)
  nested_refs.each { |nested| queue << nested unless seen.key?(nested) }
end

filtered = source.slice("swagger", "info", "host", "basePath", "schemes")
filtered["info"] = filtered.fetch("info").merge(
  "title" => "Veriphy BankWizard and IDAML API",
  "description" => "Filtered from https://test.veriphy.co.uk/api/docs/v1 for UntangleDev.Veriphy."
)
filtered["paths"] = paths
filtered["definitions"] = seen.sort.to_h

puts JSON.pretty_generate(filtered)
