describe('add wrong users', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for manager user', () => {
    cy.login('super');
  });

  it('Navigate users index page', () => {
    cy.visit('Users/Create');
    cy.get('#FirstName').type('Test');
    cy.get('#MiddleName').type('16:00');
    cy.get('#LastName').type('User');
    cy.get('#Birthday').type('2000-03-20');
    cy.get('#ZipCode').type('5500 el');
    cy.get('#Email').type('Test@user.nl');
    cy.get('#PhoneNumber').type('31677889900');
    cy.get('input[type="submit"]').should('be.visible').click();
    cy.get('#HouseNumber-error').should('be.visible');
  });
});
