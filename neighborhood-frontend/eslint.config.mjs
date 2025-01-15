import globals from "globals"
import pluginJs from "@eslint/js"
import tseslint from "typescript-eslint"
import pluginReact from "eslint-plugin-react"

/** @type {import('eslint').Linter.Config[]} */
export default [
    { extends: "airbnb" },
    { files: ["**/*.{js,mjs,cjs,ts,jsx,tsx}"] },
    { languageOptions: { globals: globals.browser } },
    pluginJs.configs.recommended,
    ...tseslint.configs.recommended,
    pluginReact.configs.flat.recommended,
    {
        rules: {
            quotes: ["error", "double", { avoidEscape: true }],
        },
    },
]
