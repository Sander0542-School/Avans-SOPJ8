// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

Cypress.Commands.add("consent", () => {
  cy.setCookie('.AspNet.Consent', 'yes');
})

Cypress.Commands.add("login", (type) => {
  cy.consent();

  cy.visit('/Identity/Account/Login');

  cy.fixture(`${type}-login`).then((login) => {
    cy.get('#Input_Email').type(login.credentials.email);
    cy.get('#Input_Password').type(login.credentials.password);
    cy.get('button[type=submit]').click();
  });

  cy.get('a[href=\'#accountSubmenu\']').should('exist');
})

Cypress.Commands.add("logout", () => {
  cy.visit('/');
  cy.get('a[href=\'#accountSubmenu\']').click();
  cy.wait(200);
  cy.get('a[href*=\'Logout\']').scrollIntoView().should('be.visible').click();
  cy.get('a[href=\'#accountSubmenu\']').should('not.exist');
})
