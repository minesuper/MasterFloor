   M S B A M L   `   `   `  ����F  BMasterFloor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:MasterFloor.PagesMasterFloor.Pages  5      #    MasterFloor.Pages.HistoryView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:MasterFloor.Pages     5        N� Title$  HistoryView��5	      .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      $��60 �6   6*   .+�5   
   �� -   5         ��Name$ EmptyHistoryTB��5      $ ��История пуста!��6,   $��Center=�6B   $��Center=�6]   
�������$��42(�6�   6�   �� 5   
   -   5         ��Name$ HistoryListView��5      $��Center=�6,   $y�Hidden=�6P   ��5      �� 5      .R�5      �� $��Center=�6   $��20q�6:   
�������$��1q�6Y   $	��600��6m   $	��150��6y   ._�5      � $��20q�6   $
��Left=�6+   .+�5      �� $��0��6)   $��Center=�69   $��Center=�6T   .+�5   "   ��  �� TextAlignment$
 Left=�6,   $��16(�6A   $��0 0 0 10q�6O   $
��Wrap=�6b   �� 5   $   $�Наименование:��6(   6?    �� 5   $   �6(   �� *Products.Name+6I   5   #   �� 5   "   $
 Left=�6,   $��16(�6A   $��0 0 0 10q�6O   $
��Wrap=�6a   �� 5   $   $�Количество:��6(   6=    �� 5   $   �6(   �� *Count+6A    �� 5    &   $�шт.��6*   67   5!   #   �� 5"   "   $
 Left=�6,   $��16(�6A   $
��Wrap=�6O   �� 5#   $   $�Дата продажи:��6(   6?    �� 5$   $   �6(   �� *
SaleDate+ �� StringFormat$ d��6T   5%   #   5&      5'      5(      5)      5+      �� 5-   
   -   5         ��Name$ 
BackButton��5-      $��
Назад��6>   $
��Left=�6N   $��20 0 0 0q�6i   $��Center=�6{   $��1��6�   6�   5.      5/      