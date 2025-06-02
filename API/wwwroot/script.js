const API_URL = "http://localhost:5084/api/livros";

window.onload = () => carregarLivros();

async function carregarLivros() {
  const resposta = await fetch(API_URL);
  const livros = await resposta.json();

  const lista = document.getElementById("listaLivros");
  lista.innerHTML = "";

  livros.forEach(livro => {
    const item = document.createElement("li");
    item.textContent = `[${livro.id}] ${livro.titulo} - ${livro.autor} (R$ ${livro.preco})`;

    // Botão de excluir
    const botaoExcluir = document.createElement("button");
    botaoExcluir.textContent = "Excluir";
    botaoExcluir.style.marginLeft = "10px";
    botaoExcluir.onclick = () => excluirLivro(livro.id);

    item.appendChild(botaoExcluir);
    lista.appendChild(item);
  });
}

async function excluirLivro(id) {
  const confirmar = confirm("Deseja excluir este livro?");
  if (!confirmar) return;

  const resposta = await fetch(`${API_URL}/${id}`, {
    method: "DELETE"
  });

  if (resposta.ok) {
    alert("Livro excluído com sucesso!");
    carregarLivros();
  } else {
    alert("Erro ao excluir livro.");
  }
}

document.getElementById("livroForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const titulo = document.getElementById("titulo").value;
  const autor = document.getElementById("autor").value;
  const preco = parseFloat(document.getElementById("preco").value);

  const novoLivro = { titulo, autor, preco };

  const resposta = await fetch(API_URL, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(novoLivro)
  });

  if (resposta.ok) {
    alert("Livro adicionado com sucesso!");
    carregarLivros();
    document.getElementById("livroForm").reset();
  } else {
    alert("Erro ao adicionar livro.");
  }
});

//Função para editar os livros

document.getElementById("editarLivro").addEventListener("submit", async function (e) {
  e.preventDefault();

  const id = parseInt(document.getElementById("idEditar").value);
  const titulo = document.getElementById("tituloEditar").value;
  const autor = document.getElementById("autorEditar").value;
  const preco = parseFloat(document.getElementById("precoEditar").value);

  const livroEditado = { id, titulo, autor, preco };

  try {
    const resposta = await fetch(`${API_URL}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(livroEditado)
    });

    if (resposta.ok) {
      alert("Livro editado com sucesso!");
      carregarLivros();
      document.getElementById("editarLivro").reset();
    } else if (resposta.status === 404) {
      alert("Livro não encontrado para edição.");
    } else {
      alert("Erro ao editar livro.");
    }
  } catch (error) {
    alert("Erro de conexão: " + error.message);
  }
}); 

//GET por ID

document.getElementById("buscarLivroForm").addEventListener("submit", async function (e) {
  e.preventDefault();

  const id = parseInt(document.getElementById("id").value);
  const resultadoDiv = document.getElementById("resultadoBusca");
  resultadoDiv.innerHTML = ""; // Limpa resultado anterior

  try {
    const resposta = await fetch(`${API_URL}/${id}`);

    if (resposta.ok) {
      const livro = await resposta.json();
      resultadoDiv.innerHTML = `
        <p><strong></strong>[${livro.id}] ${livro.titulo} - ${livro.autor} (R$${livro.preco})</p>
      
      `;
    } else if (resposta.status === 404) {
      resultadoDiv.innerHTML = `<p style="color:red;">Livro não encontrado.</p>`;
    } else {
      resultadoDiv.innerHTML = `<p style="color:red;">Erro ao buscar livro.</p>`;
    }
  } catch (error) {
    resultadoDiv.innerHTML = `<p style="color:red;">Erro de conexão: ${error.message}</p>`;
  }
});

