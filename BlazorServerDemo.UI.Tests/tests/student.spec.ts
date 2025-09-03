import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('https://localhost:7048/student');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Students/);
});

test('confirm elements', async ({ page }) => {
  await page.goto('https://localhost:7048/student');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Students/);

  await page.getByRole('button', { name: 'Update' }).isVisible()
  await page.getByRole('button', { name: 'Cancel' }).isVisible()

  await expect(page.getByRole('textbox', { name: 'Search name or email...'})).toBeVisible()
});

test('weather link', async ({ page }) => {
  await page.goto('https://localhost:7048');

  // Click the get started link.
  await page.getByRole('link', { name: 'Students' }).click();

  // Expects page to have a heading with the name of Installation.
  //await expect(page.getByRole('heading', { name: 'Installation' })).toBeVisible();
});
