namespace Assignment3.Entities;
using Assignment3.Core;

public class TagRepository : ITagRepository
{

    KanbanContext _context;
    public TagRepository(KanbanContext _context){
        this._context = _context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag) {
        var t = new Tag() {Name = tag.Name};
        _context.Tags.Add(t);
        return (Response.Created, (int)t.Id);
    }
    public IReadOnlyCollection<TagDTO> ReadAll() {
        return null;
    }
    public TagDTO Read(int tagId) {
        return null;
    }
    public Response Update(TagUpdateDTO tag) {
        return Response.Conflict;
    }
    public Response Delete(int tagId, bool force = false) {
        return Response.Conflict;
    }
}
