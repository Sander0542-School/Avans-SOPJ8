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
    cy.fixture('admin-login').then((adminLogin) => {
      cy.get('#Input_Email').type(adminLogin.credentials.email);
      cy.get('#Input_Password').type(adminLogin.credentials.password);
      cy.get('button[type=submit]').click();
    });

    cy.get('a[href=\'#accountSubmenu\']').should('exist');
  });

  it('Logout as logged in user', () => {
    cy.visit('/');
    cy.get('a[href=\'#accountSubmenu\']').click();
    cy.get('a[href*=\'Logout\']').scrollIntoView().should('be.visible').click();
    cy.get('a[href=\'#accountSubmenu\']').should('not.exist');
  });
});
