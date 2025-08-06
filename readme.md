# Aspire PoC

## Setup

- Aspire Project which builds the required services
- Web API
- Blazor frontend

### Aspire Project

- Basic Aspire setup
- End to end testing available in Aspire.Tests

### Web app

- OpenAPI explorer with Scalar (https://localhost:7515/scalar/v1)
- Reads/Writes to Postgres (localhost:5432, password from aspire dashboard)
- Writes to RabbitMQ (http://localhost:53354/#/) using transactional outbox to ensure messages are evented
  - Background task in Hangfire (https://localhost:7515/hangfire) processes messages and tries to event
  - If message fails to publish 5 times the job stops processing them
  - Messages can be retried manually with the outbox admin endpoint
- Projects have a public contracts and an internal "core" project 
  - The internal core projects are referenced **ONLY** by the main project which calls the config in their service startup
  - Core projects can only reference the contract projects which contain the interfaces
  - This allows for easier referencing where there would usually be circular references
  - This also allows projects to be swapped out, e.g. Data could have a Data.Core (Postgres), then if a swap was needed to SQLServer a Data.SqlServer could be written and swapped in while both exist using a feature flag at startup time
  - Classes in Core projects should be at most internal!
- Optional<T> if something may not exist Optional<T> should be used, the only way to get the results out here is to call `Match` with a `success` and `failure` function meaning 


### Blazor frontend

- Not much work done here yet