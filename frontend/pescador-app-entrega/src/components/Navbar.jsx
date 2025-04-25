import React from 'react';
import { Link } from 'react-router-dom';
import { FaFish } from 'react-icons/fa';
import logo from '../../public/Logo.png'; // ajuste o caminho conforme sua estrutura

const Navbar = () => {
  return (
    <nav className="bg-slate-300 p-2 text-white">
      <div className="max-w-7xl mx-auto flex justify-between items-center">
        <div className="flex items-center">
          <Link to="/">
            <img src={logo} alt="Rede de Pescadores" className="h-30 w-20" />
          </Link>
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
