import { useEffect, useState } from "react";
import { type Todo, getTodos, addTodo as apiAddTodo, updateTodo as apiUpdateTodo, deleteTodo as apiDeleteTodo } from "./api/api";
import TodoForm from "./components/TodoForm";
import TodoList from "./components/TodoList";
import "./App.css";

export default function App() {
  const [todos, setTodos] = useState<Todo[]>([]);

  // Cargar tareas desde el backend
  useEffect(() => {
    const fetchData = async () => {
      try {
        const data = await getTodos();
        setTodos(data);
      } catch (error) {
        console.error("Error al cargar tareas:", error);
      }
    };
    fetchData();
  }, []);

  // Crear tarea
  const addTodo = async (title: string, description?: string) => {
    try {
      const newTodo = await apiAddTodo(title, description);
      setTodos([...todos, newTodo]);
    } catch (error) {
      console.error("Error al agregar tarea:", error);
    }
  };

  // Cambiar estado
  const toggleTodo = async (id: number) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;
    const updated = { ...todo, isCompleted: !todo.isCompleted } as Todo;
    try {
      await apiUpdateTodo(updated);
      setTodos(todos.map((t) => (t.id === id ? updated : t)));
    } catch (error) {
      console.error("Error al actualizar tarea:", error);
    }
  };

  // Editar tarea
  const editTodo = async (id: number, newText: string, newDescription?: string) => {
    const todo = todos.find((t) => t.id === id);
    if (!todo) return;
    const updated = { ...todo, title: newText, description: newDescription } as Todo;
    try {
      await apiUpdateTodo(updated);
      setTodos(todos.map((t) => (t.id === id ? updated : t)));
    } catch (error) {
      console.error("Error al editar tarea:", error);
    }
  };

  // Eliminar tarea
  const deleteTodo = async (id: number) => {
    try {
      await apiDeleteTodo(id);
      setTodos(todos.filter((t) => t.id !== id));
    } catch (error) {
      console.error("Error al eliminar tarea:", error);
    }
  };

  return (
    <div className="app-container">
      <div className="left-panel">
        <h2>📝 Mis Tareas</h2>
        <TodoList
          todos={todos}
          toggleTodo={toggleTodo}
          deleteTodo={deleteTodo}
          editTodo={editTodo}
        />
      </div>

      <div className="right-panel">
        <h2>➕ Añadir Tarea</h2>
        <TodoForm addTodo={addTodo} />
      </div>
    </div>
  );
}
