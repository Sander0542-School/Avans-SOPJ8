describe('Approve Schedule', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for manager user', () => {
    cy.login('employee');
  });

  it('Navigate to shift offers', () => {
    cy.visit('Branches/1/Schedule/Offers');
  });

  it('Cancel offer', () => {
    cy.get('table tr input[type="submit"]:first').should('be.visible').click();

    cy.get('.alert.alert-success').should('exist');
  })
});
