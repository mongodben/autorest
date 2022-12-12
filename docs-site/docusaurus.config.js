// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require("prism-react-renderer/themes/github");
const darkCodeTheme = require("prism-react-renderer/themes/dracula");

/** @type {import('@docusaurus/types').Config} */
const config = {
  title: "AutoRest Documentation",
  tagline: "Generate client libraries for RESTful APIs",
  // TODO: add whatever URL
  url: "https://example.com",
  baseUrl: "/",
  onBrokenLinks: "throw",
  onBrokenMarkdownLinks: "warn",
  favicon: "img/logo.png",

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: "Azure", // Usually your GitHub org/user name.
  projectName: "autorest", // Usually your repo name.

  // Even if you don't use internalization, you can use this field to set useful
  // metadata like html lang. For example, if your site is Chinese, you may want
  // to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: "en",
    locales: ["en"],
  },

  presets: [
    [
      "classic",
      /** @type {import('@docusaurus/preset-classic').Options} */
      ({
        docs: {
          sidebarPath: require.resolve("./sidebars.js"),
          routeBasePath: "/",
          // TODO: Please change this to your repo.
          // Remove this to remove the "edit this page" links.
          editUrl: "https://github.com/Azure/autorest/tree/main/docs-site",
        },
        blog: false,
        theme: {
          customCss: require.resolve("./src/css/custom.css"),
        },
      }),
    ],
  ],

  themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
    ({
      navbar: {
        title: "AutoRest",
        logo: {
          alt: "AutoRest Logo",
          src: "img/logo.png",
        },
        items: [
          {
            href: "https://github.com/Azure/autorest",
            label: "GitHub",
            position: "right",
          },
        ],
      },
      footer: {
        style: "dark",
        links: [
          {
            title: "Docs",
            items: [
              {
                label: "AutoRest Documentation",
                to: "/",
              },
              {
                label: "Introduction",
                to: "/introduction",
              },
              {
                label: "Install",
                to: "/install",
              },
            ],
          },
          {
            title: "Github",
            items: [
              {
                label: "Repository",
                to: "https://github.com/Azure/autorest",
              },
              {
                label: "Submit an Issue",
                to: "https://github.com/Azure/autorest/issues",
              },
              {
                label: "License (MIT)",
                to: "https://github.com/Azure/autorest/blob/main/LICENSE",
              },
            ],
          },
        ],
      },
      prism: {
        theme: lightCodeTheme,
        darkTheme: darkCodeTheme,
      },
    }),
};

module.exports = config;
