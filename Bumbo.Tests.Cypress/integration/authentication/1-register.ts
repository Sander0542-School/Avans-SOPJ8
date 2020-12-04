function fillInRegistrationForm(formData: object) {
  cy.get('input').each((inputField, i) => {
    const inputValue = formData[Object.keys(formData)[i]];
    if (inputValue !== undefined) {
      cy.wrap(inputField).type(inputValue);
    }
  });
}

describe('Registration', () => {
  it('Check for basic elements', () => {
    cy.visit('/Identity/Account/Register');

    cy.get('input').should('exist');

    cy.get('#logoutForm').should('not.exist');
  });

  it('register user', () => {
    cy.visit('/Identity/Account/Register');

    cy.fixture('admin-login').then(((adminLogin) => {
      fillInRegistrationForm(adminLogin.credentials);

      cy.get('form > .btn').click();

      cy.location('pathname').should('contain', 'Identity/Account/RegisterConfirmation');

      cy.get('#confirm-link').click();
    }));
  });

  it('show warning on invalid data', () => {
    cy.visit('/Identity/Account/Register');

    cy.fixture('admin-login').then(((adminLogin) => {
      fillInRegistrationForm(adminLogin.invalidCredentials);

      cy.get('form > .btn').click();

      cy.location('pathname').should('not.contain', 'Identity/Account/RegisterConfirmation');
      cy.get('.validation-summary-errors').should('exist');
      cy.get('.validation-summary-errors > *').should('exist');
    }));
  });
});
