function fillInRegistrationForm(formData: object) {
  cy.get('input').each((inputField, i) => {
    const inputValue = formData[Object.keys(formData)[i]];
    if (inputValue !== undefined) {
      cy.wrap(inputField).type(inputValue);
    }
  });
}

describe('Registration', () => {
  beforeEach(() => {
    cy.visit('/Identity/Account/Register');
  });

  it('Check for basic elements', () => {
    cy.get('input').should('exist');
    cy.get('#logoutForm').should('not.exist');
  });

  it('register user', () => {
    cy.fixture('admin-login').then(((adminLogin) => {
      // Fill in form and submit
      fillInRegistrationForm(adminLogin.credentials);
      cy.get('form > .btn').click();
      // Validate if you're on email confirm page
      cy.location('pathname').should('contain', 'Identity/Account/RegisterConfirmation');
      // Confirm email
      cy.get('#confirm-link').click();
    }));
  });

  it('show warning on invalid data', () => {
    cy.fixture('admin-login').then(((adminLogin) => {
      // Fill in form and submit
      fillInRegistrationForm(adminLogin.invalidCredentials);
      cy.get('form > .btn').click();

      // Check if user still on registration page
      cy.location('pathname').should('contain', '/Identity/Account/Register');

      // Check for warning tooltips
      cy.get('#Input_Email-error').should('exist');
      cy.get('#Input_Password-error').should('exist');
      cy.get('#Input_ConfirmPassword-error').should('exist');
    }));
  });
});
