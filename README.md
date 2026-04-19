gerenciamento de inventário chamado Xpem Inventory, construído como uma aplicação web moderna. Resumo:

Stack e Arquitetura
Blazor WebAssembly (.NET 9) — frontend SPA rodando no browser
PWA — tem service worker, então pode funcionar offline
Arquitetura em camadas bem definida:
InventoryWeb — UI (páginas, componentes, layout)
Services — lógica de negócio
ApiRepos — acesso à API (padrão repository)
Models — modelos e DTOs compartilhados
Funcionalidades principais
Autenticação

Login, cadastro e recuperação de senha
JWT com persistência via localStorage
Provider de autenticação customizado
Inventário

CRUD completo de itens com upload de imagens
Paginação e busca/filtro de itens
Categorias e subcategorias
Situações dos itens (com agrupamento por quantidade)
Tipos de aquisição
UI

Sistema de notificações toast
Menu de navegação com toggle
Validação de formulários
Comunicação com API
O frontend consome uma API REST externa, cujo endereço é configurado via appsettings.json. Tem handlers HTTP customizados que injetam o token JWT automaticamente nas requisições, e suporte a upload de arquivos (imagens dos itens).

Em resumo: é o frontend de um sistema de controle de patrimônio/estoque, provavelmente parte de uma solução maior onde existe uma API separada no backend.
