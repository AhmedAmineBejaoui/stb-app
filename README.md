# STB App — Angular & .NET Full-Stack Project

A web application repository combining an **Angular 16 frontend** with a **.NET backend** and SQL database scripts. It demonstrates cross-stack application organization and API-oriented development.

## Technology overview

| Layer | Stack / files |
| --- | --- |
| Frontend | Angular 16, TypeScript, RxJS |
| UI | Tailwind CSS, Bootstrap Icons |
| Backend | .NET solution `StbBackend.sln`, `Stb.Api/`, `backend/` |
| Database | `database.sql` |

## Local development

**Frontend prerequisites:** Node.js and npm.

```bash
git clone https://github.com/AhmedAmineBejaoui/stb-app.git
cd stb-app
npm install
npm start
```

Angular serves the frontend on `http://localhost:4200/` by default. Backend setup and database configuration must be completed separately before using API-backed features.

## Available frontend commands

```bash
npm run build  # Production frontend build
npm test       # Angular unit-test runner
npm run watch  # Watch-mode build
```

For the backend, open `StbBackend.sln` in a compatible .NET environment, inspect its configuration, and run the appropriate backend project. The repository contains multiple backend-related directories; confirm which one is intended before deploying.

## Repository map

- `src/` — Angular frontend
- `Stb.Api/` and `backend/` — backend implementation
- `StbBackend.sln` — .NET solution
- `database.sql` — SQL schema/data script

## Engineering focus

This project illustrates a multi-technology application rather than a standalone Angular template. The repository has not been independently deployment- or security-audited.

**Author:** [Ahmed Amin Bejaoui](https://github.com/AhmedAmineBejaoui)
