using System.Collections.Generic;

public interface ICRUD<T>
{
    List<T> ObtenerTodos();
    T ObtenerPorId(int id);
    int Insertar(T entidad);
    int Modificar(T entidad);
    int Eliminar(int id);
}
