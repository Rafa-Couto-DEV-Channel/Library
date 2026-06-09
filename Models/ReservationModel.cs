/* Namespace funciona como o "endereço" de uma classe. Com ele, podemos ter várias classes
   com o mesmo nome sem conflito, desde que estejam em namespaces diferentes.
   
   É muito comum em projetos que usam Arquitetura Hexagonal, onde as camadas
   Domain, Data e Infrastructure ficam separadas em projetos distintos.
*/

namespace Library.API.Models;

/* Classes são as "casas" dentro desse endereço. Cada classe tem um único propósito
   e deve se manter fiel a ele — sem acumular responsabilidades que não são suas.
   
   Esse é o princípio do S no SOLID: Single Responsibility (Responsabilidade Única).
*/
public class ReservationModel
{
/*
 Por ser um Model, essa classe terá apenas propriedades públicas. Mas por que públicas?
 Porque precisamos que elas fiquem acessíveis para o Controller conseguir lê-las
 e usá-las na hora de salvar os dados no banco.
 
 Se fossem privadas, o Controller simplesmente não enxergaria essas propriedades.
*/
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTime? CreatedAt;
    public DateTime? UpdatedAt;
    public DateTime? DeletedAt;
    public ICollection<BookModel>? Books { get; set; }
    public ClientModel? Client;
}