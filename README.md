# User Directory

Small full-stack User Directory application built to the requirements, using Clean Architecture.

## Structure

- `backend/src/UserDirectory.Domain` — entities and domain rules.
- `backend/src/UserDirectory.Application` — use cases, DTOs, validation, and ports.
- `backend/src/UserDirectory.Infrastructure` — EF Core + SQLite persistence.
- `backend/src/UserDirectory.Api` — HTTP API, OpenAPI/Swagger, optional JWT/OIDC auth.
- `backend/tests/UserDirectory.UnitTests` — backend unit tests.
- `frontend` — React + TypeScript + Vite application with Add/List pages and frontend tests.

## API

- `GET /api/users`
- `GET /api/users/{id}`
- `POST /api/users`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`
- Swagger: `/swagger`

SQLite database defaults to `/data/app.db` in containers and is configurable through `ConnectionStrings__DefaultConnection`.

## Security

The API supports optional OAuth2/OIDC JWT validation using `Authentication:Enabled`, `Authentication:Authority`, and `Authentication:Audience`. The Add page is protected in the frontend when `VITE_AUTH_ENABLED=true`; List remains public by design. In production, configure an OIDC provider such as Microsoft Entra ID or Auth0.

## Run locally

### Backend

```bash
dotnet restore backend/UserDirectory.sln
dotnet run --project backend/src/UserDirectory.Api
```

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Set `VITE_API_BASE_URL` if the API is not on `http://localhost:5000`.

### Docker

```bash
docker compose up --build
```

Frontend: `http://localhost:3000`
API/Swagger: `http://localhost:5000/swagger`

## Tests

```bash
dotnet test backend/UserDirectory.sln
cd frontend
npm test -- --run
```
