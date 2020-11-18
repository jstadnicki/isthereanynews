resource "random_password" "itan-mssql-admin-password" {
  length = 32
  special = false
  override_special = "#$%&-_={}<>"
  depends_on = [azurerm_key_vault.itan-key-vault]
}

resource "random_password" "itan-mssql-reader-password" {
  length = 32
  special = false
  override_special = "#$%&-_={}<>"
  depends_on = [azurerm_key_vault.itan-key-vault]
}

resource "random_password" "itan-mssql-writer-password" {
  length = 32
  special = false
  override_special = "#$%&-_={}<>"
  depends_on = [azurerm_key_vault.itan-key-vault]
}