describe('Update Password', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for employee user', () => {
    cy.login('employee');
  });

  it('Update password', () => {
    cy.fixture('employee-login').then((employee) => {
      cy.visit(employee.passwordUrl);

      cy.get('#Input_OldPassword').clear().type(employee.credentials.password);
      cy.get('#Input_NewPassword').clear().type(employee.credentials.password);
      cy.get('#Input_ConfirmPassword').clear().type(employee.credentials.password);
    });

    cy.get('#change-password-form button[type="submit"]').click();

    cy.get('.alert.alert-success').should('exist');
  });
});
