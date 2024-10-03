[![CI](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml/badge.svg)](https://github.com/OuterlimitsTech/olt-dotnet-core/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=OuterlimitsTech_olt-dotnet-core&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=OuterlimitsTech_olt-dotnet-core)

## Exception Classes

### OltException

_General Exception_

### OltBadRequestException

_Cannot process a request due to an error_


### OltValidationException

_Data or input provided fails to meet the predefined criteria or rules established by the system or application_

### OltRecordNotFoundException

_Record Not Found_

## Other Classes

### OltValidationError (IOltValidationError)

_General validation error message class_


### OltErrorHttp (IOltErrorHttp)

_General error message class for Http Responses (used for frontend standardization)_