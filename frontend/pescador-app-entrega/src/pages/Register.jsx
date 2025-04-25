import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { FaEnvelope, FaPhone, FaLock, FaUser, FaUserTag, FaImage, FaFish } from 'react-icons/fa';


const Register = () => {
  const [imagePreview, setImagePreview] = useState(null);
  const { register, handleSubmit, formState: { errors }, watch } = useForm();

  const onSubmit = (data) => {
    console.log(data);
    // Aqui você implementaria a lógica de registro
    // Exemplo: chamar uma API de registro
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen   bg-[url('https://img.freepik.com/fotos-gratis/celebracao-fotorrealista-do-dia-do-atum-selvagem_23-2151307841.jpg?t=st=1745597855~exp=1745601455~hmac=cc2ffbab6d82c548a3e128600900ad9d93c4955246960015284fdf80f8005fa1&w=1380')] bg-cover bg-center bg-no-repeat">
      <div className="bg-white p-8 rounded-lg shadow-lg w-full max-w-md">
        <h2 className="text-xl font-semibold mb-4 text-gray-700">Registro</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaUser className="inline-block mr-2" /> Nome Completo
            </label>
            <input
              type="text"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Seu nome completo"
              {...register('name', { 
                required: 'Nome é obrigatório',
                minLength: {
                  value: 3,
                  message: 'O nome deve ter pelo menos 3 caracteres'
                }
              })}
            />
            {errors.name && <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaEnvelope className="inline-block mr-2" /> Email
            </label>
            <input
              type="email"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Seu email"
              {...register('email', { 
                required: 'Email é obrigatório',
                pattern: {
                  value: /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
                  message: 'Email inválido'
                }
              })}
            />
            {errors.email && <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaPhone className="inline-block mr-2" /> Telefone
            </label>
            <input
              type="tel"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Seu telefone (ex: 11999999999)"
              {...register('phone', { 
                required: 'Telefone é obrigatório',
                pattern: {
                  value: /^[0-9]{10,11}$/,
                  message: 'Telefone inválido'
                }
              })}
            />
            {errors.phone && <p className="text-red-500 text-sm mt-1">{errors.phone.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaLock className="inline-block mr-2" /> Senha
            </label>
            <input
              type="password"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Sua senha"
              {...register('password', { 
                required: 'Senha é obrigatória',
                minLength: {
                  value: 6,
                  message: 'A senha deve ter pelo menos 6 caracteres'
                },
                pattern: {
                  value: /^(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{6,})/,
                  message: 'A senha deve conter pelo menos um número e um caractere especial'
                }
              })}
            />
            {errors.password && <p className="text-red-500 text-sm mt-1">{errors.password.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaLock className="inline-block mr-2" /> Confirmar Senha
            </label>
            <input
              type="password"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              placeholder="Confirme sua senha"
              {...register('confirmPassword', { 
                required: 'Confirmação de senha é obrigatória',
                validate: value => value === watch('password') || 'As senhas não coincidem'
              })}
            />
            {errors.confirmPassword && <p className="text-red-500 text-sm mt-1">{errors.confirmPassword.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaUserTag className="inline-block mr-2" /> Função
            </label>
            <select
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              {...register('role', { required: 'Função é obrigatória' })}
            >
              <option value="">Selecione uma função</option>
              <option value="PESCADOR">Pescador</option>
              <option value="CONSUMIDOR">Comprador</option>
            </select>
            {errors.role && <p className="text-red-500 text-sm mt-1">{errors.role.message}</p>}
          </div>

          <div className="mb-4">
            <label className="block text-sm font-medium text-gray-600 mb-2">
              <FaImage className="inline-block mr-2" /> Foto de Perfil (opcional)
            </label>
            <input
              type="file"
              className="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-green-700"
              accept="image/*"
              onChange={handleImageChange}
              {...register('image')}
            />
            {imagePreview && (
              <div className="mt-4 text-center">
                <img 
                  src={imagePreview} 
                  alt="Preview" 
                  className="max-w-full max-h-48 object-contain rounded-lg" 
                />
              </div>
            )}
          </div>

          <button type="submit" className="w-full bg-green-800 text-white py-2 rounded-lg hover:bg-blue-900 focus:outline-none focus:ring-2 focus:ring-green-700">
            Registrar
          </button>
        </form>

        <div className="text-center mt-4">
          <Link to="/login" className="text-sm text-green-700 hover:underline">
            Já tem uma conta? Faça login
          </Link>
        </div>
      </div>
    </div>
  );
};

export default Register;
