# Architecture

The backend follows Clean Architecture dependency direction:

`Api -> Application -> Domain`

`Infrastructure -> Application + Domain`

Application contains the use-case/service boundary and repository port. Infrastructure owns EF Core and SQLite. API contains HTTP concerns only and performs dependency injection and auth configuration.

The frontend is intentionally small: API access is isolated in `src/api`, while pages own page-level state and reusable UI lives under `src/components`.
