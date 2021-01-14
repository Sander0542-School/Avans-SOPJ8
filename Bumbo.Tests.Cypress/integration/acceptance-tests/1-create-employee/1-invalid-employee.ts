describe('Fill in invalid employee data', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
    cy.visit('Users/Create');
  });

  it('Login for manager user', () => {
    cy.login('super');
  });

  it('Fill in form with invalid data', () => {
    cy.get('#HouseNumber').type('-1');
    cy.get('#ZipCode').type('123 abc');
    cy.get('#Email').type('invalid email');

    cy.get('input[type=submit]').click();

    cy.get('#Email-error').should('exist');
    cy.get('#HouseNumber-error').should('exist');
    cy.get('#ZipCode-error').should('exist');
  });
});
