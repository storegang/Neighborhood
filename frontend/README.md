# Next.js Neighborhood Frontend 🌟

This is a Next.js project for the Neighborhood Frontend, using `pnpm` as the package manager. Here you will find everything you need to set up and run the project locally.

## Prerequisites 🛠️

Before getting started, ensure you have the following installed on your machine:

- **Node.js** (version 18 or later, 20 recommended)
- **pnpm** (version 9.5.0 or later) - you can install it globally with `npm install -g pnpm`

## Installation 🖥️

Follow these steps to set up the project locally:

1. Clone the repository:

   ```bash
   git clone https://github.com/storegang/Neighborhood.git
   ```

2. Navigate to the frontend directory:

   ```bash
   cd frontend
   ```

3. Install dependencies with `pnpm`:

   ```bash
   pnpm install
   ```

## Running the Application ▶️

To start the project locally, run the following command:

```bash
pnpm dev
```

This will start a development server at `http://localhost:3000`. Open your browser and navigate there to see the application.

## Running Tests ✅

To run tests (if available), use the following command:

```bash
pnpm test
```

## GitHub Actions 🤖

This project includes a GitHub Action to automate tasks such as building and deploying the application. The workflow is triggered on pushes to specific paths and handles tasks such as:

- Installing dependencies
- Running tests
- Building the project
- Deploying to Azure App Service

For more details, check the `.github/workflows/build-push-frontend.yml` file in the repository.

## Technologies Used 🛠️

- **Next.js**: The React framework for production
- **pnpm**: Fast and efficient package manager
- **TailwindCSS**: Utility-first CSS framework
- **TypeScript**: Typed JavaScript for improved developer experience
- **Prettier**: Opinionated code formatter
- **ESLint**: Linter for identifying and fixing code issues
- **Azure**: Deployment platform (optional)
