describe('Login', () => {
  beforeEach(() => {
    cy.visit('/Identity/Account/Login');

    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Check for basic elements', () => {
    cy.get('input').should('exist');

    cy.get('#logoutForm').should('not.exist');
  });

  it('Login for existing user', () => {
    cy.login('admin');
  });

  it('Logout as logged in user', () => {
    cy.logout();
  });
});
