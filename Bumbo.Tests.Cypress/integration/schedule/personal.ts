describe('Personal', () => {

  it('Login for existing user', () => {
    cy.visit('/Identity/Account/Login');

    cy.fixture('admin-login').then((adminLogin) => {
      cy.get('#Input_Email').type(adminLogin.credentials.email);
      cy.get('#Input_Password').type(adminLogin.credentials.password);
      cy.get('button[type=submit]').click();
    });

    cy.get('a[href=\'#accountSubmenu\']').should('exist');
  });


  it('Has calendar', () => {
    cy.visit('/Branches/1/Schedule/Personal/');
    cy.get('#calendar').should('exist');
    cy.get('.fc-body').should('exist');
  });
});
