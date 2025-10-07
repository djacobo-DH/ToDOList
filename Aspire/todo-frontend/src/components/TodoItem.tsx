import { useState } from "react";
import { type Todo } from "../types";

interface Props {
  todo: Todo;
  toggleTodo: (id: number) => void;
  deleteTodo: (id: number) => void;
  editTodo: (id: number, newText: string, newDescription?: string) => void;
}

export default function TodoItem({ todo, toggleTodo, deleteTodo, editTodo }: Props) {
  const [isEditing, setIsEditing] = useState(false);
  const [title, setTitle] = useState(todo.title);
  const [description, setDescription] = useState(todo.description ?? "");

  const handleSave = () => {
    editTodo(todo.id, title, description);
    setIsEditing(false);
  };

  return (
    <div className="todo-item">
      <input type="checkbox" checked={todo.isCompleted} onChange={() => toggleTodo(todo.id)} />

      {isEditing ? (
        <div className="flex-1">
          <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} />
          <textarea value={description} onChange={(e) => setDescription(e.target.value)} />
        </div>
      ) : (
        <div className="flex-1">
          <div className="font-medium">{todo.title}</div>
          {todo.description && <div className="text-sm text-gray-600">{todo.description}</div>}
        </div>
      )}

      <div className="actions">
        {isEditing ? (
          <button onClick={handleSave} title="Guardar">💾</button>
        ) : (
          <button onClick={() => setIsEditing(true)} title="Editar">✏️</button>
        )}

        <button onClick={() => deleteTodo(todo.id)} title="Eliminar">🗑️</button>
      </div>
    </div>
  );
}
