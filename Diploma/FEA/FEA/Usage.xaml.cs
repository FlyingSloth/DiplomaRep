using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FEA
{
	/// <summary>
	/// Interaction logic for Usage.xaml
	/// </summary>
	public partial class Usage : Window
	{
		public Usage()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			textBlock2.Text = "Общие характеристики:" + Environment.NewLine + 
								Environment.NewLine + 
								"Характеристики слоёв:" + Environment.NewLine +
"                               Число слоёв: целое число между 1 и 20;" + Environment.NewLine +
"                               Радиусы: числа, показывающие расстояние между центром волновода и границей слоя;" + Environment.NewLine +
"                               Диэлектрическая проницаемость." + Environment.NewLine +
							    Environment.NewLine +
								"Номер моды: целое число между 1 и 20;" + Environment.NewLine +
							    Environment.NewLine +
								"Число конечных элементов: целое число между 200 и 1100 - для работы программы с достаточной точностью, но без перегрузки процессора." + Environment.NewLine + 
								Environment.NewLine +
								"Дисперсионные характеристики(зависимость постоянной распространения от частоты):" + Environment.NewLine +
"                               Число шагов волнового числа;" + Environment.NewLine +
"                               Размер шага волнового числа." + Environment.NewLine +
							    Environment.NewLine +
								"Критические условия предельного затухания (для двуслойного волновода):" + Environment.NewLine +
"                               анализ дисперсионных характеристик в зависимости от радиуса внутреннего слоя.";

			textBlock3.Text = "    Все данные вводятся в главном окне. Характеристики вводятся в соответсвующие поля через разделитель '|'. Каждому полю соответствует подсказка."
				+ Environment.NewLine +
				"   После ввода данных надо нажать кнопку \"Рассчитать\", в результате чего появляется окно, показывающее текущий прогресс рассчётов. Вычисления могут быть довольно длительными. Вычисления могут быть прерваны кнопкой \"Отмена\". Эта кнопка возвращает на начальную страницу, на которой можно изменить характеристики волновода либо завершить программу."
				+ Environment.NewLine +
				"   Когда таймер показывает, что процесс завершился, нажмите \"Ok\" для просмотра результатов."
				+ Environment.NewLine +
				"   В итоге, можно видеть данные, отображённые на графиках, данные и графики можно сохранить (кнопка \"Сохранить\").";
		}
	}
}
