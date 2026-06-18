# Library API

Projeto didático para explicar os principais conceitos de Orientação a Objetos — aqueles que exigem planejamento antes de sair codando.

---

## Conceitos de P.O.O aplicados neste projeto

### 1. Class (Classe)

Uma **classe** é um molde que define quais dados e comportamentos um objeto vai ter. Pense nela como a planta de um apartamento: a planta não é o apartamento, mas descreve como ele deve ser construído.

No projeto, `BookModel` é uma classe que representa um livro no banco de dados:

```csharp
// Models/BookModel.cs
public class BookModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
```

Cada propriedade (`Title`, `Author`, etc.) é um **dado** que todo objeto do tipo `BookModel` vai carregar.

---

### 2. Record

Um **record** é parecido com uma classe, mas foi projetado para representar dados imutáveis — ou seja, um objeto que só existe para transportar informação de um lugar para outro. No C#, records têm igualdade por valor (dois records com os mesmos dados são considerados iguais) e são ideais para **DTOs** (Data Transfer Objects).

Aqui usamos `record` para os dados que chegam na requisição HTTP:

```csharp
// DTOs/BookDto.cs
public record BookDto
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
```

Repare na diferença de propósito:
- `BookModel` → representa o livro **no banco de dados** (tem `Id`, `CreatedAt`, etc.)
- `BookDto` → representa os dados que o **usuário envia** na requisição (só o que ele precisa preencher)

---

### 3. Interface

Uma **interface** é um contrato. Ela define quais métodos uma classe deve ter, sem implementar nenhum deles. Quem assina o contrato (a classe) é obrigada a cumprir tudo que está nele.

A grande vantagem é que o código que usa a interface não precisa saber qual classe está por trás — só precisa saber que o contrato será respeitado.

```csharp
// Repository/BookRepository.cs
public interface IBookRepository
{
    Task<List<BookModel>> GetAllBook(CancellationToken ct);
    Task<BookModel> GetBookById(Guid bookId, CancellationToken ct);
    Task CreateBook(BookModel payload, CancellationToken ct);
    Task DeleteBook(Guid bookId, CancellationToken ct);
    Task UpdateBook(Guid bookId, BookModel payload, CancellationToken ct);
}
```

A interface `IBookRepository` diz: *"qualquer repositório de livros precisa ter esses 5 métodos"*. A classe `BookRepository` então assina esse contrato com `: IBookRepository`:

```csharp
public class BookRepository : IBookRepository
{
    // implementa os 5 métodos obrigatórios...
}
```

---

### 4. Method (Método)

Um **método** é uma ação que um objeto sabe executar. É dentro dos métodos que fica o comportamento — a lógica do que o sistema faz.

O método abaixo busca todos os livros que não foram deletados:

```csharp
// Repository/BookRepository.cs
public async Task<List<BookModel>> GetAllBook(CancellationToken ct = default)
{
    return await _context.Book
        .Where(d => d.DeletedAt == null)
        .ToListAsync(ct);
}
```

Anatomia do método:
- `async Task<List<BookModel>>` → tipo de retorno (uma lista de livros, de forma assíncrona)
- `GetAllBook` → nome do método
- `(CancellationToken ct)` → parâmetro que permite cancelar a operação
- O corpo executa a consulta no banco e retorna o resultado

---

### 5. Class Constructor (Construtor)

O **construtor** é um método especial executado automaticamente quando um objeto é criado. Ele serve para preparar o objeto — geralmente, recebendo e guardando as dependências de que ele precisa para funcionar.

O construtor de `BookRepository` recebe o contexto do banco de dados:

```csharp
// Repository/BookRepository.cs
public class BookRepository : IBookRepository
{
    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    private readonly AppDbContext _context;
    // ...
}
```

Quando o .NET cria um `BookRepository`, ele executa esse construtor, injeta o `AppDbContext` e o guarda em `_context`. A partir daí, todos os métodos da classe podem usar `_context` para acessar o banco.

---

### 6. Dependency Injection (Injeção de Dependência)

**Injeção de Dependência** é o mecanismo que entrega automaticamente as dependências de que uma classe precisa. Em vez de a classe criar suas próprias dependências com `new`, ela as *recebe* de fora — geralmente pelo construtor.

O `BookController` precisa de três dependências para funcionar. Ele não as cria; ele as pede:

```csharp
// Controllers/BookController.cs
public class BookController : ControllerBase
{
    public BookController(
        IBookRepository bookRepository,
        IValidator<BookDto> bookValidator,
        ILogger<BookController> logger
    )
    {
        this._bookRepository = bookRepository;
        this._bookValidator = bookValidator;
        this._logger = logger;
    }

    private readonly IBookRepository _bookRepository;
    private readonly IValidator<BookDto> _bookValidator;
    private readonly ILogger<BookController> _logger;
}
```

Para que isso funcione, as dependências precisam ser **registradas** no container de DI. Isso é feito no `Program.cs`:

```csharp
// Program.cs
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<IBookRepository>(sp => sp.GetRequiredService<BookRepository>());
```

Fluxo completo:
1. O `Program.cs` registra `BookRepository` como implementação de `IBookRepository`
2. Quando uma requisição chega, o .NET cria um `BookController`
3. O construtor do controller pede um `IBookRepository`
4. O .NET consulta o registro, encontra `BookRepository` e o injeta automaticamente

---

### 7. S — Single Responsibility Principle (Princípio da Responsabilidade Única)

O **S** do SOLID diz: *cada classe deve ter uma única razão para mudar*. Em outras palavras, cada peça do código deve fazer uma coisa só, e fazê-la bem.

Este projeto aplica esse princípio separando responsabilidades em camadas distintas:

| Camada | Classe | Responsabilidade única |
|---|---|---|
| Models | `BookModel` | Representar o livro no banco de dados |
| DTOs | `BookDto` | Transportar dados da requisição HTTP |
| Validators | `BookValidator` | Validar se os dados do `BookDto` são válidos |
| Repository | `BookRepository` | Executar operações no banco de dados |
| Controller | `BookController` | Receber a requisição, orquestrar e devolver a resposta |

**Exemplo prático:** se amanhã a regra de validação do título mudar (ex: mínimo de 3 caracteres), você só precisa mexer no `BookValidator` — nenhuma outra classe precisa ser alterada. Se a forma de salvar no banco mudar, só o `BookRepository` é afetado.

```csharp
// Validators/BookValidator.cs — responsabilidade única: validar BookDto
public class BookValidator : AbstractValidator<BookDto>
{
    public BookValidator()
    {
        RuleFor(p => p.Title)
            .NotNull().WithMessage("Title is null")
            .NotEmpty().WithMessage("Title is empty");

        RuleFor(p => p.Author)
            .NotNull().WithMessage("Author is null")
            .NotEmpty().WithMessage("Author is empty");

        RuleFor(p => p.Description)
            .NotNull().WithMessage("Description is null")
            .NotEmpty().WithMessage("Description is empty");
    }
}
```

Essa separação torna o código mais fácil de ler, testar e manter — porque cada arquivo tem um propósito claro.
