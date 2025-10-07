import { type Todo } from "../types";

// 🌐 Base URL configurable (Vite o valor por defecto)
const API_BASE: string =
  (import.meta as any).env?.VITE_API_BASE ?? "https://localhost:5010";

// ✅ Endpoint base real
const API_URL = `${API_BASE}/api/todo`;

export async function getTodos(): Promise<Todo[]> {
  const res = await fetch(API_URL);
  if (!res.ok) throw new Error("Error al obtener tareas");
  return await res.json();
}

export async function addTodo(title: string, description?: string): Promise<Todo> {
  const res = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title, description: description ?? null, isCompleted: false }),
  });
  if (!res.ok) throw new Error("Error al agregar tarea");
  return await res.json();
}

export async function updateTodo(todo: Todo): Promise<void> {
  const res = await fetch(`${API_URL}/${todo.id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      id: todo.id,
      title: todo.title,
      description: todo.description,
      isCompleted: todo.isCompleted,
    }),
  });
  if (!res.ok) throw new Error("Error al actualizar tarea");
}

export async function deleteTodo(id: number): Promise<void> {
  const res = await fetch(`${API_URL}/${id}`, { method: "DELETE" });
  if (!res.ok) throw new Error("Error al eliminar tarea");
}

export type { Todo };
