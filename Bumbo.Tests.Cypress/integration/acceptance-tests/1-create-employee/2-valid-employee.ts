describe('Fill in valid employee data', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login as super user', () => {
    cy.login('super');
  });

  it('Fill in form with valid data and submit', () => {
    cy.visit('Users/Create');
    cy.get('#FirstName').type('first');
    cy.get('#LastName').type('last');
    cy.get('#Birthday').type('1990-10-10');
    cy.get('#ZipCode').type('1234 ab');
    cy.get('#HouseNumber').type('10');
    cy.get('#Email').type('employee2@bumbo.test');
    cy.get('#PhoneNumber').type('0600000052');
    cy.get('input[type=submit]').click();
  });

  it('Check if data has been added', () => {
    cy.visit('Users');
    cy.get('tbody').should('contain', 'employee2@bumbo.test');
  });
});
