"use client";

import {
  BarChart,
  Compass,
  Layout,
  List,
  Layers,
  GraduationCap,
  BookOpenText,
  School,
  Wallet2,
  WalletCards,
} from "lucide-react";
import { usePathname } from "next/navigation";
import SidebarItem from "./sidebar-item";
import { useEffect, useState } from "react";

const guestRoutes = [
  {
    icon: Compass,
    label: "Browse",
    href: "/home",
  },
  {
    icon: Wallet2,
    label: "Payments",
    href: "/payments",
  }
];

const userRoutes = [
  {
    icon: Compass,
    label: "Browse",
    href: "/home",
  },
  {
    icon: Wallet2,
    label: "Payments",
    href: "/payments",
  }
];


const adminRoutes = [
  {
    icon: Layout,
    label: "Dashboard",
    href: "/admin/dashboard",
  },
  // {
  //   icon: WalletCards,
  //   label: "Payout Management",
  //   href: "/admin/payout",
  // },
  {
    icon: Layers,
    label: "Packages Management",
    href: "/admin/package",
  },
  {
    icon: GraduationCap,
    label: "Student Management",
    href: "/admin/student",
  },
  // {
  //   icon: School,
  //   label: "Instructor Management",
  //   href: "/admin/instructor",
  // },
  // {
  //   icon: BookOpenText,
  //   label: "Course Management",
  //   href: "/admin/course",
  // },
];

export const SidebarRoutes = () => {
  const pathName = usePathname();

  let routes;

  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    if (typeof window !== "undefined") {
      const storedUserData = localStorage.getItem("userData");
      if (storedUserData) {
        setIsLoggedIn(true);
      }
    }
  }, []);

  if (pathName?.includes("/admin")) {
    routes = adminRoutes;
  } else if (isLoggedIn) {
    routes = userRoutes;
  } else {
    routes = guestRoutes;
  }

  return (
    <div className="flex flex-col w-full">
      {routes.map((route) => (
        <SidebarItem
          key={route.href}
          icon={route.icon}
          label={route.label}
          href={route.href}
        />
      ))}
    </div>
  );
};
