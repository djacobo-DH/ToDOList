import { useState } from "react";

interface Props {
  addTodo: (title: string, description?: string) => void;
}

export default function TodoForm({ addTodo }: Props) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (title.trim()) {
      addTodo(title.trim(), description.trim() || undefined);
      setTitle("");
      setDescription("");
    }
  };

  return (
    <form onSubmit={handleSubmit} className="todo-form">
      <input
        type="text"
        placeholder="Título de la tarea"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        className="mb-2"
      />

      <textarea
        placeholder="Descripción (opcional)"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        className="mb-2"
      />

      <button type="submit">Agregar</button>
    </form>
  );
}
