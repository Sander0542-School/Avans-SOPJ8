describe("Homepage", () => {
  it("Has navbar", () => {
    cy.visit("/");
    cy.get(".navbar-nav").should('exist');
  })
})