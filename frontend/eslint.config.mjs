import globals from "globals"
import pluginReact from "eslint-plugin-react"
import tseslint from "@typescript-eslint/eslint-plugin"
import pluginPrettier from "eslint-plugin-prettier"
import pluginNext from "@next/eslint-plugin-next"

/** @type {import('eslint').Linter.FlatConfig[]} */
export default [
    // TypeScript Configuration
    {
        files: ["**/*.{ts,tsx}"],
        languageOptions: {
            parser: "@typescript-eslint/parser",
            parserOptions: {
                ecmaVersion: "latest",
                sourceType: "module",
            },
        },
        rules: {
            ...tseslint.configs.recommended.rules,
        },
    },
    // React Configuration
    {
        files: ["**/*.{jsx,tsx}"],
        plugins: {
            react: pluginReact,
        },
        rules: {
            ...pluginReact.configs.recommended.rules,
        },
    },
    // Global Settings and Overrides
    {
        files: ["**/*.{js,mjs,cjs,ts,jsx,tsx}"],
        languageOptions: {
            globals: globals.browser, // eller globals.node avhengig av miljø
        },
        rules: {
            quotes: ["error", "double", { avoidEscape: true }],
        },
    },
    // Prettier Integration
    {
        files: ["**/*.{js,ts,jsx,tsx}"],
        plugins: {
            prettier: pluginPrettier,
        },
        rules: {
            "prettier/prettier": "error", // Ensure Prettier issues are treated as errors
        },
    },
    // Next.js Configuration
    {
        files: ["**/*.{js,ts,jsx,tsx}"],
        plugins: {
            next: pluginNext,
        },
        rules: {
            "next/no-img-element": "warn", // Example of Next.js specific rule
        },
    },
]
