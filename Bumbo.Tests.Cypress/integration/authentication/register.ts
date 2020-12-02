describe("Registration", () => {
  it("Check for basic elements", () => {
    cy.visit("/Identity/Account/Register");

    cy.get("input").should("exist");

    cy.get("#logoutForm").should("not.exist");
  })

  it("Can register new user", () => {
    cy.visit("/Identity/Account/Register");

  });


  it("Should register user", () => {
    cy.visit("/Identity/Account/Register");

    cy.fixture("admin-login").then((adminLogin => {
      // Fill in input fields
      cy.get("input").each((inputField, i) => {
        let inputValue = adminLogin.credentials[Object.keys(adminLogin.credentials)[i]];
        if(inputValue !== undefined) {
          cy.wrap(inputField).type(inputValue);
        }
      });

      cy.get("form > .btn").click();

      cy.location('pathname').should('contain', "Identity/Account/RegisterConfirmation")

      cy.get("#confirm-link").click();
  }))});
});