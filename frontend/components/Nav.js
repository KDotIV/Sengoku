import Link from 'next/link';

export default function Nav() {
  return (
    <nav>
      <Link href="/events">Events</Link>
      <Link href="/merch">Merch</Link>
      <Link href="/orders">Orders</Link>
      <Link href="/account">Account</Link>
    </nav>
  );
}
