   M S B A M L   `   `   `  ����F  BSecurityBag, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:SecurityBagSecurityBag  5   	       SecurityBag.ChangePass   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35` \RibbonControlsLibrary, Version=3.5.41019.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:SecurityBag     5   	   -   5          � Title$  
ChangePass��5   	   $��260.105��6    � WindowStartupLocation$ CenterScreen=�6-   $	��470��6R    � AllowsTransparency . 
������  � WindowStyle$
 None=�6�   .��5	      � .+�5
   
   �� 
�������$
��Left=�6!   $	��249��6<   $��	10,10,0,0q�6I    �� RadiusY$ 10��6\    �� RadiusX$ 10��6i   ���	#FFBCBCBC$	��Top=�6�   $	��450��6�   �� 5   
   $
��Left=�6    J� Opacity$	 0.3Q�6+   $��img/back2.png��69   $	��191��6P   $��	10,56,0,0q�6]   $	��Top=�6p   $	��450��6�   �� 5   
   -   5        $
��Left=�5      $��46��6/   $��	10,10,0,0q�6;   $ 10��6N   $ 10��6[   ���	#FFC7C7C7$	��Top=�6{   $	��450��6�   ��5      �� 5       �� ImageSource$ img/header2.png��6   5      �� 5   
   -   5        $
��Left=�5      $��11��6+    � Cursor$
 Hand��67   $��img/closeicon.png��6E   $	 0.5Q�6`   $��	53,30,0,0q�6n   $	��Top=�6�   $��13��6�   6�   �� 5   
   $��تغییر رمزعبور��6   $
��Left=�6(   $��
314,22,0,0q�6C   $	��Top=�6W   $��B Yekan)�6o   $��14(�6�   �� 5   
   ���	#FFF4F4F4$
��Left=�6%   $��33��6@   $��
10,224,0,0q�6L   $ 10��6`   $ 10��6m   ���	#FFC2C2C2$	��Top=�6�   $	��450��6�   �� 5   
   -   5        $��ثبت��5      	 �� 	IsDefault	 . $
��Left=�61   $��349,228,0,0q�6L   $	��Top=�6a   $��75��6y   $��25��6�   ���	#FFC0C02D
�������6�   �� 5   
   -   5        $��انصراف��5   #   
 �� IsCancel
 . $
��Left=�6D   $��269,227,0,0q�6_   $	��Top=�6t   $��75��6�   $��25��6�   �� 5   
   $��رمز عبور:��6   $
��Left=�6$   $��RightToLeft=�6?   $��
330,76,0,0q�6[   $	��Top=�6o   �� 5   
   $��رمز عبور جدید:��6   $
��Left=�6)   $��RightToLeft=�6D   $��330,125,0,0q�6`   $	��Top=�6u   �� 5   
   $*��$تکرار رمز عبور جدید:��6   $
��Left=�6/   $��RightToLeft=�6J   $��330,156,0,0q�6f   $	��Top=�6{   G� 5   
   -   5        $��txtPass��5   2   $
��Left=�6   $��0��6A   $��RightToLeft=�6N   $��23��6j   $��
119,78,0,0q�6v   $	��Top=�6�   $	��156��6�   G� 5   
   -   5        $��
txtNewPass��5   1   $
��Left=�6   $��1��6C   $��RightToLeft=�6P   $��23��6l   $��119,128,0,0q�6x   $	��Top=�6�   $	��156��6�   G� 5   
   -   5         G�Name$ txtNewPass2��5   1   $
��Left=�6   $��1��6F   $��RightToLeft=�6S   $��23��6o   $��119,158,0,0q�6{   $	��Top=�6�   $	��156��6�   5"      5#      