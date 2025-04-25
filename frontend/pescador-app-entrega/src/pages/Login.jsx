import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { FaEnvelope, FaPhone, FaLock, FaFish } from 'react-icons/fa';


const Login = () => {
  const [loginMethod, setLoginMethod] = useState('email');
  const { register, handleSubmit, formState: { errors } } = useForm();

  const onSubmit = (data) => {
    console.log(data);
    // Aqui você implementaria a lógica de autenticação
    // Exemplo: chamar uma API de autenticação
  };

  return (
    <div className="flex items-center justify-center min-h-screen  px-4 bg-[url('https://img.freepik.com/fotos-gratis/celebracao-fotorrealista-do-dia-do-atum-selvagem_23-2151307841.jpg?t=st=1745597855~exp=1745601455~hmac=cc2ffbab6d82c548a3e128600900ad9d93c4955246960015284fdf80f8005fa1&w=1380')] bg-cover bg-center bg-no-repeat">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-2xl font-semibold text-gray-800 mb-4 text-center">Login</h2>
        
        <div className="flex justify-between mb-4">
          <button 
            type="button"
            className={`w-1/2 py-2 text-lg font-medium text-gray-700 ${loginMethod === 'email' ? 'border-b-2 border-green-700 text-blue-700' : ''}`}
            onClick={() => setLoginMethod('email')}
          >
            Email
          </button>
          <button 
            type="button"
            className={`w-1/2 py-2 text-lg font-medium text-gray-700 ${loginMethod === 'phone' ? 'border-b-2 border-green-700  text-blue-700' : ''}`}
            onClick={() => setLoginMethod('phone')}
          >
            Telefone
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          {loginMethod === 'email' ? (
            <div className="mb-4">
              <label className="block text-lg font-medium text-gray-700 mb-2">
                <FaEnvelope className="inline-block mr-2" /> Email
              </label>
              <input
                type="email"
                className="w-full p-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
                placeholder="Seu email"
                {...register('email', { 
                  required: 'Email é obrigatório',
                  pattern: {
                    value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                    message: 'Email inválido'
                  }
                })}
              />
              {errors.email && <p className="text-red-600 text-sm mt-1">{errors.email.message}</p>}
            </div>
          ) : (
            <div className="mb-4">
              <label className="block text-lg font-medium text-gray-700 mb-2">
                <FaPhone className="inline-block mr-2" /> Telefone
              </label>
              <input
                type="tel"
                className="w-full p-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-700"
                placeholder="Seu telefone (ex: 11999999999)"
                {...register('phone', { 
                  required: 'Telefone é obrigatório',
                  pattern: {
                    value: /^[0-9]{10,11}$/,
                    message: 'Telefone inválido'
                  }
                })}
              />
              {errors.phone && <p className="text-red-600 text-sm mt-1">{errors.phone.message}</p>}
            </div>
          )}

          <div className="mb-4">
            <label className="block text-lg font-medium text-gray-700 mb-2">
              <FaLock className="inline-block mr-2" /> Senha
            </label>
            <input
              type="password"
              className="w-full p-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Sua senha"
              {...register('password', { 
                required: 'Senha é obrigatória',
                minLength: {
                  value: 6,
                  message: 'A senha deve ter pelo menos 6 caracteres'
                }
              })}
            />
            {errors.password && <p className="text-red-600 text-sm mt-1">{errors.password.message}</p>}
          </div>

          <button type="submit" className="w-full py-3 bg-green-800 text-white text-lg rounded-lg hover:bg-green-900 focus:outline-none focus:ring-2 focus:ring-blue-800">
            Entrar
          </button>
        </form>

        <Link to="/register" className="block text-center text-green-700 mt-4 text-sm hover:text-green-800">
          Não tem uma conta? Registre-se
        </Link>
      </div>
    </div>
  );
};

export default Login;
