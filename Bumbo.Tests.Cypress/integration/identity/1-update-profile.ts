describe('Update Profile', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for employee user', () => {
    cy.login('employee');
  });

  it('Update profile', () => {
    cy.fixture('employee-login').then((employee) => {
      cy.visit(employee.profileUrl);

      cy.get('#Input_Birthday').clear().type(employee.newProfile.birthday);
      cy.get('#Input_PhoneNumber').clear().type(employee.newProfile.phoneNumber);
      cy.get('#Input_ZipCode').clear().type(employee.newProfile.zipCode);
      cy.get('#Input_HouseNumber').clear().type(employee.newProfile.houseNumber);
    });

    cy.get('#profile-form button[type="submit"]').click();

    cy.get('.alert.alert-success').should('exist');
  });
});
