# Changelog

## 1.1.0

- Added idiomatic BankWizard and IDAML request models for new code.
- Added clean facade overloads that map to the existing Veriphy transfer objects.
- Added `IdAmlServices` service-code constants with idiomatic names.
- Clarified that this is an unofficial, unaffiliated client and is not endorsed by Veriphy.
- Kept generated DTO overloads and response types available for compatibility.

## 1.0.0

- Initial SDK package for Veriphy BankWizard and IDAML.
- Added a generated OpenAPI contract snapshot filtered to `/BankWizard`, `/IDAML`, and `/IDAML/MONITOR`.
- Added facade clients, DI registration, and typed service-code constants.
