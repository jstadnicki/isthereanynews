resource "azurerm_key_vault_secret" "clientid" {
  name = "clientid"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = azuread_application.itan-ad-application-api.application_id
}

resource "azurerm_key_vault_secret" "itan-secret-client-policy" {
  name = "AzureAdB2C--Policy"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "B2C_1_itansignup"
}

resource "azurerm_key_vault_secret" "itan-secret-scope-read" {
  name = "AzureAdB2C--ScopeRead"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = var.scope-reader
}

resource "azurerm_key_vault_secret" "itan-secret-scope-write" {
  name = "AzureAdB2C--ScopeWrite"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = var.scope-writer
}

resource "azurerm_key_vault_secret" "itan-secret-tenant" {
  name = "AzureAdB2C--Tenant"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = var.tenant-id
}

resource "azurerm_key_vault_secret" "itan-secret-storage" {
  name = "ConnectionStrings--Storage"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = azurerm_storage_account.itan_storage_account.primary_access_key
  
  depends_on = [azurerm_storage_account.itan_storage_account]
}

resource "azurerm_key_vault_secret" "itan-secret-sql-reader" {
  name = "ConnectionStrings--SqlReader"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "server=.;database=itan;User Id=itanreaduser;password=${random_password.itan-mssql-reader-password.result}"
}

resource "azurerm_key_vault_secret" "itan-secret-sql-writer" {
  name = "ConnectionStrings--SqlWriter"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "server=.;database=itan;User Id=itanwriteuser;password=${random_password.itan-mssql-writer-password.result}"
}
