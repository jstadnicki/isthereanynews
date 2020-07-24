resource "azurerm_key_vault_secret" "clientid" {
  name = "clientid"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = azuread_application.example.application_id
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


resource "azurerm_key_vault_secret" "itan-secret-emulator" {
  name = "ConnectionStrings--emulator"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;"
}


resource "azurerm_key_vault_secret" "itan-secret-sql-reader" {
  name = "ConnectionStrings--SqlReader"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "server=.;database=itan;User Id=itanreaduser;password=12qw!@QW"
}

resource "azurerm_key_vault_secret" "itan-secret-sql-writer" {
  name = "ConnectionStrings--SqlWriter"
  key_vault_id = azurerm_key_vault.itan-key-vault.id
  value = "server=.;database=itan;User Id=itanwriteuser;password=12qw!@QW"
}
