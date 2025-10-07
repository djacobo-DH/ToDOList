import {type Todo } from "../api/api";
import TodoItem from "./TodoItem";

interface Props {
  todos: Todo[];
  toggleTodo: (id: number) => void;
  deleteTodo: (id: number) => void;
  editTodo: (id: number, newText: string, newDescription?: string) => void;
}

export default function TodoList({ todos, toggleTodo, deleteTodo, editTodo }: Props) {
  return (
    <div className="todo-list">
      {todos.length === 0 ? (
        <p>No hay tareas aún 😴</p>
      ) : (
        todos.map((todo) => (
          <TodoItem
            key={todo.id}
            todo={todo}
            toggleTodo={toggleTodo}
            deleteTodo={deleteTodo}
            editTodo={editTodo}
          />
        ))
      )}
    </div>
  );
}
