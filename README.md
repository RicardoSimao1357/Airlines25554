# Airlines25554
 
 ### TEMA / DOMÍNIO: UFCD – 5414/5417 Programação Web – Servidor / Cliente – CET69
 
 ### FORMADOR: Rafael Santos
 
 
• No âmbito das unidades de formação Programação Web – Server side – e Client side, propõem-se a conceção de uma aplicação web asp.net de um sistema de informação
para uma companhia aeronáutica.
 
## • A referida aplicação deverá fazer uso de:

-> ASP.NET Core com arquitetura MVC (Model-View-Controller) com autenticação;

-> Padrão repository;

-> Entity Framework Core para a criação e toda a gestão da base de dados em SQLServer;

-> Repositório no github.

_______________________________________________________________________________________________________________________________________________________________________
 
## • Funcionalidades gerais:

-> Implementação de todos os CRUD’s;

-> Sistema completo de autenticação (login/logout, registo de novos utilizadores, recuperação de password por email, etc);

-> Criação de Roles com um mínimo de 4 tipos de utilizadores (administrador de toda a plataforma, funcionário, cliente e utilizador anónimo);

->  Utilização de pelo menos dois controlos de terceiros que não se deverão repetir entre formandos (syncfusion por exemplo);

-> A aplicação deverá estar funcional e online quando for testada e apresentada;

-> A aplicação não pode em caso algum rebentar nem mostrar os ecrãs de erros de desenvolvimento, devendo criar todas as views para a gestão de erros e conflitos dos crud’s.

-> Sistema de front-end original e adaptado ao projeto.

_____________________________________________________________________________________________________________________________________________________________________

## • Funcionalidades especificas:

-> Criação de voos, com número de voo, data, hora, origem, destino e aparelho;

-> Consoante os tipos de aparelho disponível deverão ser criados os lugares para venda;

-> Depois do voo criado deverá ser possível comprar bilhetes bem como escolher o lugar pretendido, devendo depois de escolhido, ficar indisponível para os 
próximos compradores.

-> Poderão também ser feitas consultas aos voos existentes por data, origem ou ambos.

________________________________________________________________________________________________________________________________________________________________

O administrador do sistema apenas gere os funcionários, criando-lhes as contas para estes posteriormente poderem entrar na plataforma. Irá também disponibilizar os aparelhos e cidades/aeroportos (origem e destino).

Atenção que no processo de criação da conta, qualquer utilizador criado no ponto anterior, irá receber primeiro um email onde deverá alterar a sua password.

Deverá também haver um utilizador funcionário, que será responsável pela criação dos voos com o respetivo aparelho. Esses voos deverão depois estar disponíveis para poderem sercomprados pelos clientes.

Finalmente irão existir os clientes que, se poderão registar quando quiserem, ou automaticamente quando adquirem o primeiro voo.

Os clientes terão ainda acesso ao histórico das suas viagens com a companhia.

Os utilizadores anónimos podem consultar os voos disponíveis e também os comprar, sendo automaticamente criada a conta de cliente depois de efetuada a compra.

____________________________________________________________________________________________________________________________________________________________________
 
 
 ## • Matriz de funcionalidades :
 
|Funcionalidade|Admin|Funcionário|Cliente|Anónimo|
|:-----|:---:|:---:|:---:|:---:|
|Login|X|X|X||
|Criar contas a funcionários|X||||
|CRUD de Aviões e Cidades|X||||
|CRUD de voos||x|||
|CRUD de Clientes|x|||
|Consultas de voos||x|x|x
|Modificar perfil|x|x|x|
|Recuperar Password|x|x|x|
|Consultar histórico de voos||x|x|
|Consultar e alterar dados de voos||x||
|Visualizar informações sobre os voos disponíveis|x|x|x|x

____________________________________________________________________________________________________________________________________________________________________

 

## • Outros aspetos relevantes:

-> Todo o utilizador com exceção dos anónimos deverá ser dado a possibilidade de ter foto de perfil. 

-> Os aparelhos terão de ter uma foto obrigatória, bem com deverá aparecer a bandeira referente aos países das cidades de origem e destino.

-> Na sua área o cliente poderá consultar todo o seu histórico de compras, bem como, o estado de futuros voos já comprados.

-> Deverá haver o respeito por não apagar e atualizar em cascata, sendo necessária a comunicação entre os diversos utilizadores.

-> Deverá também ser criada e publicada uma web API que envie os futuros voos já comprados pelo cliente.

______________________________________________________________________________________________________________________________________________________________________

## • Avaliação:

-> Aplicação de todas as funcionalidades mínimas descritas anteriormente – 15 valores

-> Commits semanais (todas as segundas-feiras a partir do dia 5/09) – 2 valores

-> Funcionalidades extra – 3 valores

-> Será descontado 1 valor por cada dia depois do prazo de entrega.
