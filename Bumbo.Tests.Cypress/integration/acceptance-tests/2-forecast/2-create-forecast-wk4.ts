describe('Fill in valid employee data', () => {
  beforeEach(() => {
    // Keeps session alive through this "describe" method
    Cypress.Cookies.preserveOnce('.AspNetCore.Identity.Application');
  });

  it('Login as manager', () => {
    cy.login('manager');
  });

  it('Go to create forecast page', () => {
    cy.visit('Branches/1/Forecast/Create/2021/4');

    // eslint-disable-next-line no-plusplus
    for (let i = 0; i < 7; i++) {
      cy.get(`:nth-child(${2 + i}) > :nth-child(2) > #ExpectedNumberOfColi`).type('20');

      cy.get(`:nth-child(${2 + i}) > :nth-child(3) > #MetersOfShelves`).type('20');

      if (i < 5) {
        cy.get(`:nth-child(${2 + i}) > :nth-child(4) > #ExpectedVisitorPerDay`).type('800');
      } else {
        cy.get(`:nth-child(${2 + i}) > :nth-child(4) > #ExpectedVisitorPerDay`).type('1000');
      }
    }

    cy.get('input[type=submit]').click();
  });

  it('Check if table body is filled', () => {
    cy.visit('Branches/1/Forecast/Index/2021/4');

    // Loop through all table items except department names
    cy.get('tbody > tr:not(:first-child)').each((element) => {
      cy.wrap(element).should('contain.text', ':');
    });
  });
});
