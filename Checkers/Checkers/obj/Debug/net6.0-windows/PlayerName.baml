   M S B A M L   `   `   `  ????C  ?Checkers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:CheckersCheckers  5   	       Checkers.PlayerName   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Checkers     5   	     ? Title$
  Game??5   	    ? SizeToContent$ WidthAndHeight=?6    ? WindowStartupLocation$ CenterScreen=?65   .??5	      ? $??10q?6    ? RowDefinitions 5
   
   ?? 5      $??20 ?6   ?? 5      $??20 ?6   ?? 5      $
??Auto ?6   .+?5   
   ?? Enter player name:6(   ?? 5   
   -   5        $??TextBox2??5      $??1??6"   $	??250??6/   6@   ? 5   
   $??2??6   $??Right=?6!   $??0,15,0,0q?6=   .+?5      ?? -   5         ?? 	IsDefault$
 True??5      $??60??6;   $??0,0,10,0q?6I   .??6[   Ok6_   ?? 5       ?? IsCancel$
 True??6   $??60??6%   .??63   Cancel6;   5      5      5      