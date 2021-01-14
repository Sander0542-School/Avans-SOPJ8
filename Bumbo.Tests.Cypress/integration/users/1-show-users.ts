describe('show users', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for manager user', () => {
    cy.login('super');
  });

  it('Navigate users index page', () => {
    cy.visit('Users');
    cy.get('#userTable').should('exist');
  });
});
