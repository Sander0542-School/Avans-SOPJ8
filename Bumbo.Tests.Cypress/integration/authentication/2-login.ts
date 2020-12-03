describe('Login', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Check for basic elements', () => {
    cy.visit('/Identity/Account/Login');

    cy.get('input').should('exist');

    cy.get('#logoutForm').should('not.exist');
  });

  it('Login for existing user', () => {
    cy.visit('/Identity/Account/Login');

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
    cy.get('a[href*=\'Logout\']').should('be.visible').click();
    cy.get('a[href=\'#accountSubmenu\']').should('not.exist');
  });
});
