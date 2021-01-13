describe('Create Shift', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login for super user', () => {
    cy.visit('/Identity/Account/Login');

    cy.fixture('super-login').then((superLogin) => {
      cy.get('#Input_Email').type(superLogin.credentials.email);
      cy.get('#Input_Password').type(superLogin.credentials.password);
      cy.get('button[type=submit]').click();
    });

    cy.get('a[href=\'#accountSubmenu\']').should('exist');
  });
});
